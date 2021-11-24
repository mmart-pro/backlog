<template>
  <v-card class="mt-4">
    <v-row no-gutters>
      <v-sheet style="width: calc(100% - 120px);">
        <v-card-title>
          <div class="text-truncate">{{title}}</div>
        </v-card-title>
        <v-card-subtitle v-if="preview">
          <div class="text-truncate">{{preview}}</div>
        </v-card-subtitle>
      </v-sheet>
      <v-sheet
        align="end"
        class="d-flex flex-column justify-space-between pt-1 pr-1"
        style="width: 120px;"
      >
        <div>
          <PriorityItem :priority="priority" />
        </div>
        <div class="subtitle-2">
          <div>{{author}}</div>
          <div>{{createTimeStamp | dateTimeFormat}}</div>
        </div>
      </v-sheet>
    </v-row>
  </v-card>
</template>

<script>
export default {
  props: {
    title: { type: String, default: 'title' },
    content: { type: String, default: 'content' },
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
    preview() {
      return this.content?.length < 100
        ? this.content
        : this.content.substring(0, 100) + '...'
    }
  }
}
</script>
