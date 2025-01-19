<script setup lang="ts">
import { useConfigStore } from '@/stores';
import { storeToRefs } from 'pinia';
import homemessages from '@/assets/homemessages.json';

const { inDarkMode } = storeToRefs(useConfigStore());
</script>

<template>
  <div class="justify-center align-center flex-column d-flex ga-4">
    <v-card
      class="align-center d-flex"
      width="calc(100% -  64px)"
      max-width="640"
      elevation="8"
      height="50%"
      color="mix"
    >
      <v-carousel :show-arrows="false" hide-delimiters cycle>
        <v-carousel-item v-for="homemessage in homemessages" :key="homemessage.id">
          <div class="justify-center flex-column d-flex h-100">
            <v-card-title>
              {{ homemessage.title }}
            </v-card-title>
            <strong class="text-medium-emphasis">
              {{ homemessage.text }}
            </strong>
          </div>
        </v-carousel-item>
      </v-carousel>

      <v-switch
        class="position-absolute bottom-0 right-0 mr-4"
        false-icon="mdi-weather-night"
        true-icon="mdi-weather-sunny"
        v-model="inDarkMode"
      />
    </v-card>
    <v-btn text="check facts" class="px-16" color="info" to="/Facts"/>
  </div>
</template>
