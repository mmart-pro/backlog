<template>
  <v-dialog
    v-model="show"
    width="400"
  >
    <v-card>
      <v-card-title>
        <span>{{ project.id > 0 ? 'Изменение' : 'Добавление' }} проекта</span>
        <v-spacer></v-spacer>
        <v-btn
          small
          icon
          dark
          @click="cancelClick"
        >
          <v-icon>mdi-close</v-icon>
        </v-btn>
      </v-card-title>

      <v-card-text>
        <v-row
          align="center"
          justify="center"
          no-gutters
          class="mx-0"
        >
          <v-col cols="12 px-0">
            <v-text-field
              v-model="project.name"
              :error-messages="objectErrors($v.project.name)"
              :counter="$v.project.name.$params.maxLength.max"
              autocomplete="off"
              type="text"
              label="Название проекта"
              @input="$v.project.name.$touch()"
              @blur="$v.project.name.$touch()"
            ></v-text-field>
          </v-col>
        </v-row>
      </v-card-text>

      <v-card-actions class="pt-8">
        <v-spacer></v-spacer>
        <v-btn
          class="px-6"
          outlined
          @click.prevent="cancelClick"
        >Отмена</v-btn>
        <v-btn
          class="px-12"
          color="primary"
          :loading="loading"
          @click.prevent="okClick"
        >OK</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
import { required, minLength, maxLength } from 'vuelidate/lib/validators'

export default {
  props: {
    value: { type: Object, default: null }, // показать диалог если есть открываемый элемент
    saveCallback: { type: Function, default: null }
  },

  data: () => ({
    project: {},
    loading: false
  }),

  computed: {
    show: {
      get() {
        // показываем если id != 0
        return this.project.id
      },
      set(val) {
        this.$v.$reset()
        if (!val) {
          // сбрасываем объект, который передавали в диалог
          this.$emit('input', null)
        }
      }
    }
  },

  watch: {
    value(val) {
      this.project = {
        ...val,
        // если id есть, но это пустой объект, то дать ему id = -1, если null, то 0 иначе id
        id: val?.id ? val.id : val ? -1 : 0
      }
    }
  },

  validations() {
    return {
      project: {
        name: { required, minLength: minLength(6), maxLength: maxLength(30) }
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

    async okClick() {
      if (this.$v.$invalid) {
        this.$v.$touch()
        return
      }

      this.loading = true
      const res = await this.saveCallback(this.project)
      if (res) {
        this.show = false
      }
      this.loading = false
    },

    cancelClick() {
      this.show = false
    }
  }
}
</script>
