<template>
  <div>
    <template v-for="(msg, index) in messages">
      <v-snackbar
        v-if="msg.key >= 0"
        :key="index"
        :value="true"
        :color="msg.type"
        :style="`margin-bottom: ${msg.margin*60}px; height: auto;`"
        :timeout="msg.timeout"
        @input="close(msg)"
      >
        {{msg.text}}
        <template v-slot:action="{ attrs }">
          <v-btn
            v-bind="attrs"
            icon
            @click="close(msg)"
          >
            <v-icon>mdi-close-circle</v-icon>
          </v-btn>
        </template>
      </v-snackbar>
    </template>
  </div>
</template>

<script>
import Vue from 'vue'

export default {
  data: () => ({
    messages: [],
    key: 0,
    margin: 0
  }),

  created() {
    Vue.prototype.$notify = this.addMessage
  },

  methods: {
    // this.$notify('warning', 'Do you really want to exit?')
    // this.$notify('success', 'Do you really want to exit?')
    // this.$notify('info', 'Do you really want to exit?')
    // this.$notify('error', 'Do you really want to exit?')
    addMessage(type, text, timeout) {
      const msg = {
        type,
        text,
        timeout: timeout || (type === 'error' ? 8000 : 4000),
        margin: this.margin,
        key: this.key
      }
      this.messages.push(msg)
      this.key++
      this.margin++
    },

    close(msg) {
      msg.key = -1
      let margin = 0
      this.messages.forEach((m) => {
        m.margin = m.key >= 0 ? margin++ : 0
      })
      this.margin = margin
    }
  }
}
</script>
