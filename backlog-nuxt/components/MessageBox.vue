<template>
  <v-dialog
    v-model="show"
    :width="options.width"
  >
    <v-card class="dialogBackground">
      <v-card-text
        style="font-size: 1rem"
        :class="options.class"
        v-html="text"
      />
      <v-card-actions class="pt-4 justify-end">
        <v-btn
          v-if="options.cancel"
          class="px-6"
          outlined
          @click.prevent="close(false)"
        >Отмена</v-btn>
        <v-btn
          class="px-12"
          color="primary"
          @click.prevent="close(true)"
        >ОК</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
import Vue from 'vue'

const defaults = {
  width: 'unset',
  class: 'pa-4',
  cancel: true
}

export default {
  data: () => ({
    show: false,
    resolve: null,
    dialogResult: false,
    text: '',
    options: {}
  }),

  watch: {
    show(val) {
      if (val) {
        // open
        this.dialogResult = false
      } else {
        // close
        this.resolve(this.dialogResult)
      }
    }
  },

  created() {
    Vue.prototype.$messageBox = this.messageBox
  },

  methods: {
    messageBox(text, options) {
      return new Promise((resolve) => {
        this.resolve = resolve
        this.text = text
        this.options = { ...defaults, ...options }
        this.show = true
      })
    },

    close(val) {
      this.dialogResult = val
      this.show = false
    }
  }
}
</script>
