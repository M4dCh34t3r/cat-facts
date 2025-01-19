<script setup lang="ts">
import { RouterView } from 'vue-router'
import OverlayStoreDialogs from './components/misc/OverlayStoreDialogs.vue';
import { computed, onMounted } from 'vue';
import { useConfigStore, useOverlayStore } from './stores';
import { storeToRefs } from 'pinia';

const { inDarkMode } = storeToRefs(useConfigStore());

const appTheme = computed(() => (inDarkMode.value ? 'defaultDark' : 'defaultLight'));

onMounted(() => useOverlayStore().showInfoMessageDialog('OI', 'Ima info message', 500));
</script>

<template>
  <v-app :theme="appTheme">
    <v-main>
      <OverlayStoreDialogs />
      <v-btn
        append-icon="mdi-theme-light-dark"
        @click="inDarkMode = !inDarkMode"
        text="toggle theme"
        color="success"
      />
      <RouterView />
    </v-main>
  </v-app>
</template>
