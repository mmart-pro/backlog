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
        <span>
          {{project.project.name}}
        </span>
      </v-card-title>

      <v-divider />

      <v-card-subtitle>{{project.userRole.name}}</v-card-subtitle>
    </v-card>

    <template v-if="todos.length">
      <nuxt-link
        v-for="todo in todos"
        :key="todo.id"
        :to="'/todos/'+todo.id"
        style="text-decoration: none"
      >
        <Todo
          :title="todo.title"
          :content="todo.content"
          :priority="todo.priority"
          :author="todo.creator.name"
          :create-time-stamp="todo.createTimeStamp"
        />
      </nuxt-link>
    </template>
    <template v-else>
      <v-card class="mt-4">
        <v-card-text class="warning--text">
          Добавьте задачи в проект
        </v-card-text>
      </v-card>
    </template>

    <!-- кнопка добавить -->
    <v-btn
      bottom
      right
      color="primary"
      dark
      fab
      fixed
      :to="'/todos/0?projectId='+project.projectId"
    >
      <v-icon class="white--text">mdi-plus</v-icon>
    </v-btn>
  </div>
</template>

<script>
export default {
  validate({ params }) {
    return /^\d+$/.test(params.id)
  },

  async asyncData({ $axios, params }) {
    const project = await $axios.$get('/userProjects/' + params.id)
    const todos = await $axios.$get('/todos', {
      params: {
        projectId: params.id
      }
    })
    return {
      project,
      todos,
      path: [
        {
          text: 'Проекты',
          disabled: false,
          href: '/'
        },
        {
          text: params.id > 0 ? project.project.name : 'Создание проекта',
          disabled: true
        }
      ]
    }
  },

  head: { title: 'Задачи по проекту' }
}
</script>

