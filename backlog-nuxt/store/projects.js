export const state = () => ({
  items: []
})

export const mutations = {
  setProjects (state, userProjects) {
    state.items = userProjects
  },

  insert (state, userProject) {
    state.items.push(userProject)
  },

  update (state, userProject) {
    state.items = state.items.map(i => i.projectId === userProject.projectId ? userProject : i)
  },

  delete (state, projectId) {
    const index = state.items.findIndex(i => i.projectId === projectId)
    if (index !== -1)
      state.items.splice(index, 1)
  }
}

export const actions = {
  async fetch ({ commit }) {
    const userProjects = await this.$axios.$get('/userProjects')
    commit('setProjects', userProjects)
  },

  async insert ({ commit }, name) {
    const projectId = await this.$axios.$post('/userProjects', null, {
      params: {
        projectName: name
      }
    })
    const userProject = await this.$axios.$get('/userProjects/' + projectId)
    commit('insert', userProject)
  },

  async update ({ commit }, { projectId, name }) {
    await this.$axios.$put('/userProjects', { projectId, name })
    const userProject = await this.$axios.$get('/userProjects/' + projectId)
    commit('update', userProject)
  },

  async delete ({ commit }, projectId) {
    await this.$axios.$delete('/userProjects/' + projectId)
    commit('delete', projectId)
  }
}

export const getters = {
  items: s => s.items
}
