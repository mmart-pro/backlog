<template>
  <v-card>
    <v-card-title>
      <span>{{name}}</span>

      <v-spacer />

      <v-btn
        icon
        color="blue"
        @click.prevent="editClick"
      >
        <v-icon>mdi-pencil</v-icon>
      </v-btn>
      <v-btn
        icon
        color="red"
        @click.prevent="deleteClick"
      >
        <v-icon>mdi-delete</v-icon>
      </v-btn>
    </v-card-title>

    <v-divider />

    <v-card-subtitle>
      <span>{{userRole}}</span>
    </v-card-subtitle>
  </v-card>
</template>

<script>
export default {
  props: {
    name: { type: String, default: 'name' },
    userRole: { type: String, default: 'userRole' }
  },

  methods: {
    editClick() {
      this.$emit('edit')
    },

    deleteClick() {
      this.$emit('delete')
    }
  }
}
</script>
