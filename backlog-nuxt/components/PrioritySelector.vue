<template>
  <v-select
    v-model="selected"
    :disabled="disabled"
    :items="items"
    item-text="name"
    item-value="id"
    label="Приоритет"
    :error-messages="errorMessages"
    @input="onSelectedChanged"
    @blur="onBlur"
  >
    <template #selection="{ item }">
      <PriorityItem
        :priority="item"
        class="my-1"
      />
    </template>
    <template #item="{ item }">
      <PriorityItem :priority="item" />
    </template>
  </v-select>
</template>

<script>
export default {
  props: {
    value: { type: Number, default: null },
    disabled: { type: Boolean, default: false },
    errorMessages: { type: Array, default: () => [] }
  },

  data: () => ({
    selected: null
  }),

  computed: {
    items() {
      return this.$store.getters['priorities/items']
    }
  },

  watch: {
    value(val) {
      this.selected = val
    }
  },

  created() {
    this.selected = this.value
  },

  methods: {
    onSelectedChanged(val) {
      this.$emit('input', val)
    },
    onBlur(val) {
      this.$emit('blur', val)
    }
  }
}
</script>
