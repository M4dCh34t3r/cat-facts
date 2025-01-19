import './assets/main.css'

import { createApp } from 'vue'

import App from './App.vue'
import vuetify from './plugins/vuetify'
import router from './plugins/router'
import pinia from './plugins/pinia'
import axios from './plugins/axios'

const app = createApp(App)

app.use(axios)
app.use(pinia)
app.use(router)
app.use(vuetify)

app.mount('#app')
