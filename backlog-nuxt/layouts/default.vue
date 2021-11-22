<template>
  <v-app dark>
    <v-app-bar
      fixed
      app
    >
      <nuxt-link to="/">
        <span>Backlog</span>
      </nuxt-link>
    </v-app-bar>
    <v-main>

      <v-container>
        <Nuxt />
      </v-container>
    </v-main>
    <Notify />
    <MessageBox />
  </v-app>
</template>

<script>
export default {}
</script>
