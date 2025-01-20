import './assets/main.css';

import { createApp } from 'vue';

import App from './App.vue';
import axios from './plugins/axios';
import pinia from './plugins/pinia';
import router from './plugins/router';
import vuetify from './plugins/vuetify';

const app = createApp(App);

app.use(axios);
app.use(pinia);
app.use(router);
app.use(vuetify);

app.mount('#app');
