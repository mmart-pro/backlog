<template>
  <div>
    <ProjectEditDialog
      v-model="editProjectItem"
      :save-callback="onSaveProject"
    />

    <v-row justify="start">
      <v-col
        v-for="project in projects"
        :key="project.project.id"
        cols="12"
        sm="6"
        md="4"
        lg="3"
      >
        <nuxt-link
          :to="'/projects/'+project.projectId"
          class="ma-4"
          style="text-decoration: none"
        >
          <Project
            :name="project.project.name"
            :user-role="project.userRole.name"
            @edit="editClick(project.project)"
            @delete="deleteClick(project.project)"
          />
        </nuxt-link>
      </v-col>
    </v-row>

    <!-- кнопка добавить -->
    <v-btn
      bottom
      right
      color="primary"
      dark
      fab
      fixed
      :loading="loading"
      @click.prevent="addClick()"
    >
      <v-icon class="white--text">mdi-plus</v-icon>
    </v-btn>
  </div>
</template>

<script>
export default {
  // middleware: ['auth'],
  data: () => ({
    // диалог
    editProjectItem: null,
    loading: false
  }),

  async fetch({ store }) {
    if (store.getters['projects/items'].length === 0) {
      await store.dispatch('projects/fetch')
    }
  },

  head: { title: 'Проекты' },

  computed: {
    projects() {
      return this.$store.getters['projects/items']
    }
  },

  methods: {
    addClick() {
      this.editProjectItem = {}
    },

    editClick(value) {
      this.editProjectItem = value
    },

    async deleteClick(project) {
      if (
        !(await this.$messageBox(
          `Удалить проект <span class="accent--text">${project.name}</span> ?`,
          { width: '400px' }
        ))
      )
        return
      this.loading = true
      try {
        await this.$store.dispatch('projects/delete', project.id)
        this.$notify('success', 'Проект удалён')
      } catch (err) {
        this.$notify('error', 'Ошибка при удалении проекта: ' + err)
      } finally {
        this.loading = false
      }
    },

    onSaveProject(project) {
      if (project.id > 0) this.updateProject(project)
      else this.addProject(project)
    },

    async updateProject(project) {
      this.loading = true
      try {
        await this.$store.dispatch('projects/update', {
          projectId: project.id,
          name: project.name
        })
        this.$notify('success', 'Проект изменён')
        this.editProjectItem = null
      } catch (err) {
        this.$notify('error', 'Ошибка при изменении проекта: ' + err)
      } finally {
        this.loading = false
      }
    },

    async addProject(project) {
      this.loading = true
      try {
        await this.$store.dispatch('projects/insert', project.name)
        this.$notify('success', 'Проект добавлен')
        this.editProjectItem = null
      } catch (err) {
        this.$notify('error', 'Ошибка при добавлении проекта: ' + err)
      } finally {
        this.loading = false
      }
    }
  }
}
</script>
