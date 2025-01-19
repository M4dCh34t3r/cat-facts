import { createRouter, createWebHashHistory } from 'vue-router';

import HomeView from '@/views/HomeView.vue';
import FactsView from '@/views/FactsView.vue';

const routes = [
  {
    path: '/',
    name: 'Home',
    component: HomeView,
    meta: {
      title: 'Home page'
    }
  },
  {
    path: '/Facts',
    name: 'Facts',
    component: FactsView,
    meta: {
      title: 'Facts page'
    }
  }
];

const router = createRouter({
  history: createWebHashHistory(),
  routes
});

router.afterEach((to) => {
  if (typeof to.meta.title === 'string')
    document.title = to.meta.title;
});

export default router;
