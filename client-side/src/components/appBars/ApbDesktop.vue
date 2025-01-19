<script setup lang="ts">
import { useConfigStore } from '@/stores';
import { storeToRefs } from 'pinia';
import { computed } from 'vue';

const { inDarkMode } = storeToRefs(useConfigStore());

const icnThemeIcon = computed(() => (inDarkMode.value ? 'mdi-weather-sunny' : 'mdi-weather-night'));
</script>

<template>
  <v-app-bar v-if="true" align="center" rounded="0">
    <template #prepend>
      <v-btn prepend-icon="mdi-home" text="home" to="/" />
    </template>
    <img src="/title.svg" height="32px" />
    <template #append>
      <v-scale-transition leave-absolute>
        <v-icon
          @click="() => (inDarkMode = !inDarkMode)"
          :icon="icnThemeIcon"
          :key="icnThemeIcon"
          variant="text"
          size="x-large"
          class="mr-2"
        />
      </v-scale-transition>
    </template>
  </v-app-bar>
</template>

<style scoped>
.v-list {
  padding: 0;
}
</style>
