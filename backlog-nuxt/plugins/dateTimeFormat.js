import Vue from 'vue'

Vue.prototype.$dateTimeFormat = (value) => {
  if (value && value.length >= 16) {
    const date = String(value).slice(0, 16)
    return date.slice(8, 10) + '.' + date.slice(5, 7) + '.' + date.slice(0, 4) + ' ' + date.slice(11, 13) + ':' + date.slice(14, 16)
  } else return ''
}
Vue.filter('dateTimeFormat', Vue.prototype.$dateTimeFormat)
