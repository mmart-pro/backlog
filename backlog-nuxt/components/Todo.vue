<template>
  <v-card class="mt-4">
    <v-card-title>
      {{title}}
      <v-spacer />
      {{priority}}
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
    priority: { type: String, default: 'priority' },
    author: { type: String, default: 'author' },
    createTimeStamp: { type: String, default: 'date' }
  }
}
</script>
