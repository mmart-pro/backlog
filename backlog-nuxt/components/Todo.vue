<template>
  <v-card class="mt-4">
    <v-card-title>
      {{title}}
      <v-spacer />
      <v-chip
        style="cursor: pointer;"
        :color="color"
      >{{priority.name}}</v-chip>
    </v-card-title>

    <v-card-subtitle>
      <span>{{author}}</span>
      <v-spacer />
      <span>Создана {{createTimeStamp | dateTimeFormat}}</span>
    </v-card-subtitle>
  </v-card>
</template>

<script>
export default {
  props: {
    title: { type: String, default: 'title' },
    priority: {
      type: Object,
      default() {
        return { id: 0, name: 'priority' }
      }
    },
    author: { type: String, default: 'author' },
    createTimeStamp: { type: String, default: 'date' }
  },

  computed: {
    color() {
      switch (this.priority.id) {
        // low
        case 50:
          return 'grey darken-1'
        // high
        case 150:
          return 'orange darken-1'
        // critical
        case 200:
          return 'red'
        // normal
        default:
          return 'blue darken-1'
      }
    }
  }
}
</script>
