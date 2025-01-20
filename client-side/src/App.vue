<script setup lang="ts">
import { storeToRefs } from 'pinia';
import { computed, onBeforeUnmount, onMounted } from 'vue';
import { RouterView } from 'vue-router';
import ApbDesktop from './components/appBars/ApbDesktop.vue';
import ApbMobile from './components/appBars/ApbMobile.vue';
import OverlayStoreDialogs from './components/misc/OverlayStoreDialogs.vue';
import { useConfigStore } from './stores/config';

const { inDarkMode, inMobileMode, showAppBar } = storeToRefs(useConfigStore());

const appTheme = computed(() => (inDarkMode.value ? 'defaultDark' : 'defaultLight'));

function onWindowResize() {
  if (inMobileMode.value && window.innerWidth > 1024) inMobileMode.value = false;
  else if (!inMobileMode.value && window.innerWidth <= 1024) inMobileMode.value = true;
}

onBeforeUnmount(() => window.removeEventListener('resize', onWindowResize));

onMounted(async () => window.addEventListener('resize', onWindowResize));
</script>

<template>
  <v-app :theme="appTheme">
    <v-main>
      <v-fade-transition>
        <template v-if="showAppBar">
          <ApbMobile v-if="inMobileMode" />
          <ApbDesktop v-else />
        </template>
      </v-fade-transition>

      <OverlayStoreDialogs />

      <router-view v-slot="{ Component }">
        <v-fade-transition mode="out-in">
          <component :is="Component" />
        </v-fade-transition>
      </router-view>
    </v-main>
  </v-app>
</template>
