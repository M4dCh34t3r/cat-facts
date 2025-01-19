import './assets/main.css'

import { createApp } from 'vue'

import App from './App.vue'
import router from './plugins/router'
import pinia from './plugins/pinia'

const app = createApp(App)

app.use(pinia)
app.use(router)

app.mount('#app')
