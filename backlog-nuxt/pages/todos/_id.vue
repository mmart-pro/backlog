<template>
  <div>
    <v-sheet rounded>
      <v-breadcrumbs :items="path">
        <template #divider>
          <v-icon>mdi-chevron-right</v-icon>
        </template>
      </v-breadcrumbs>
    </v-sheet>

    <v-card class="mt-4">
      <v-card-title>
        <v-text-field
          v-model="item.title"
          :error-messages="objectErrors($v.item.title)"
          :counter="$v.item.title.$params.maxLength.max"
          autocomplete="off"
          label="Краткое описание задачи"
          @input="$v.item.title.$touch()"
          @blur="$v.item.title.$touch()"
        />
      </v-card-title>

      <v-card-subtitle v-if="item.id > 0">
        <span>{{item.creator.name}}</span>
        <v-spacer />
        <span>Создана {{item.createTimeStamp | dateTimeFormat}}</span>
      </v-card-subtitle>

      <v-card-text>
        <PrioritySelector v-model="item.priorityId" />

        <v-textarea
          v-model="item.content"
          filled
          label="Пояснения по задаче"
          :error-messages="objectErrors($v.item.content)"
          :counter="$v.item.content.$params.maxLength.max"
          @input="$v.item.content.$touch()"
          @blur="$v.item.content.$touch()"
        />
      </v-card-text>

      <v-card-actions>
        <v-btn
          icon
          color="primary"
          @click="backClick"
        >
          <v-icon>mdi-keyboard-backspace</v-icon>
        </v-btn>
        <v-spacer />
        <v-btn
          v-if="item.id > 0"
          icon
          color="red"
          @click="deleteClick"
        >
          <v-icon>mdi-delete</v-icon>
        </v-btn>
        <v-btn
          icon
          color="green"
          @click="saveClick"
        >
          <v-icon>mdi-check</v-icon>
        </v-btn>
      </v-card-actions>
    </v-card>
  </div>
</template>

<script>
import { required, minLength, maxLength } from 'vuelidate/lib/validators'

export default {
  validate({ params }) {
    return /^\d+$/.test(params.id)
  },

  async asyncData({ $axios, params, query }) {
    const item =
      params.id > 0
        ? await $axios.$get('/todos/' + params.id)
        : {
            id: 0,
            title: '',
            content: '',
            priorityId: 100
          }

    const project =
      params.id > 0
        ? item.project
        : (await $axios.$get('/userProjects/' + query.projectId)).project

    return {
      item,
      path: [
        {
          text: 'Проекты',
          disabled: false,
          href: '/'
        },
        {
          text: project.name,
          disabled: false,
          href: '/projects/' + project.id
        },
        {
          text: params.id > 0 ? item.title : 'Создание задачи',
          disabled: true
        }
      ]
    }
  },

  head: { title: 'Задача' },

  validations() {
    return {
      item: {
        title: { required, minLength: minLength(3), maxLength: maxLength(120) },
        content: {
          maxLength: maxLength(1000)
        }
      }
    }
  },

  methods: {
    objectErrors(obj) {
      if (!obj.$dirty) return []
      const errors = []

      obj.$params.maxLength &&
        !obj.maxLength &&
        errors.push(
          `Не более ${obj.$params.maxLength.max} символов, пожалуйста`
        )

      obj.$params.minLength &&
        !obj.minLength &&
        errors.push(
          `Не менее ${obj.$params.minLength.min} символов, пожалуйста`
        )

      obj.$params.required && !obj.required && errors.push('Введите значение')
      return errors
    },

    saveClick() {
      if (this.$v.$invalid) {
        this.$v.$touch()
        return
      }
      if (this.item.id > 0) this.update()
      else this.insert()
    },

    backClick() {
      this.$router.go(-1)
    },

    async update() {
      this.$nuxt.$loading.start()
      try {
        await this.$axios.$put('/todos', {
          todoId: this.item.id,
          priorityId: this.item.priorityId,
          title: this.item.title,
          content: this.item.content
        })
        this.$notify('success', 'Задача изменена')
        this.$router.go(-1)
      } catch (err) {
        this.$notify('error', 'Ошибка при изменении задачи: ' + err)
      } finally {
        this.$nuxt.$loading.finish()
      }
    },

    async insert() {
      if (!this.$route.query.projectId) {
        this.$notify('error', 'Ошибка: нет идентификатора проекта')
        return
      }
      this.$nuxt.$loading.start()
      try {
        await this.$axios.$post('/todos', {
          projectId: this.$route.query.projectId,
          priorityId: this.item.priorityId,
          title: this.item.title,
          content: this.item.content
        })
        this.$notify('success', 'Задача создана')
        this.$router.go(-1)
      } catch (err) {
        this.$notify('error', 'Ошибка при создании задачи: ' + err)
      } finally {
        this.$nuxt.$loading.finish()
      }
    },

    async deleteClick() {
      if (!(await this.$messageBox(`Удалить задачу ?`, { width: '400px' })))
        return
      this.$nuxt.$loading.start()
      try {
        await this.$axios.$delete('/todos/' + this.item.id)
        this.$notify('success', 'Задача удалена')
        this.$router.go(-1)
      } catch (err) {
        this.$notify('error', 'Ошибка при удалении задачи: ' + err)
      } finally {
        this.$nuxt.$loading.finish()
      }
    }
  }
}
</script>

