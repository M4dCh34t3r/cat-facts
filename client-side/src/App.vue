<script setup lang="ts">
import { RouterView } from 'vue-router';
import { computed, onBeforeUnmount, onMounted } from 'vue';
import { storeToRefs } from 'pinia';
import { useConfigStore } from './stores/config';
import OverlayStoreDialogs from './components/misc/OverlayStoreDialogs.vue';

const { inDarkMode, inMobileMode } = storeToRefs(useConfigStore());

const appTheme = computed(() => (inDarkMode.value ? 'defaultDark' : 'defaultLight'));

function onWindowResize() {
  if (inMobileMode.value && window.innerWidth > 1024)
    inMobileMode.value = false;
  else if (!inMobileMode.value && window.innerWidth <= 1024)
    inMobileMode.value = true;
}

onBeforeUnmount(
  () =>
    window.removeEventListener('resize', onWindowResize)
);

onMounted(
  async () =>
    window.addEventListener('resize', onWindowResize)
);
</script>

<template>
  <v-app :theme="appTheme">
    <v-main>
      <OverlayStoreDialogs />

      <router-view v-slot="{ Component }">
        <v-fade-transition mode="out-in">
          <component :is="Component" />
        </v-fade-transition>
      </router-view>
    </v-main>
  </v-app>
</template>
