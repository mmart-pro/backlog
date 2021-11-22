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
  ></v-select>
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
