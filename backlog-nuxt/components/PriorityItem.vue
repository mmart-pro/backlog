<template>
  <v-chip
    style="cursor: pointer;"
    :color="color"
  >{{priority.name}}</v-chip>
</template>

<script>
export default {
  props: {
    priority: {
      type: Object,
      default() {
        return {
          id: 0,
          name: 'priority'
        }
      }
    }
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
