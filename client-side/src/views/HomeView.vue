<script setup lang="ts">
import DivHomeMessages from '@/components/divs/DivHomeMessages.vue';
import { useConfigStore } from '@/stores';
import { storeToRefs } from 'pinia';
import { onBeforeUnmount, onMounted, ref } from 'vue';

const imgSlideInterval = 5000;
const imgSlideSrc = [
  'banners/black-cat.jpg',
  'banners/calico-cat.jpg',
  'banners/gray-cat.jpg',
  'banners/siamese-cat.jpg',
  'banners/white-cat.jpg',
  'banners/yellow-cat.jpg'
];

let intervalId: number | undefined;

const imgSlideSrcIndex = ref<number>(0);
const { inMobileMode, showAppBar } = storeToRefs(useConfigStore());

function slideshowStart() {
  intervalId = setInterval(() => {
    imgSlideSrcIndex.value = (imgSlideSrcIndex.value + 1) % imgSlideSrc.length;
  }, imgSlideInterval);
}

function slideshowStop() {
  if (typeof intervalId === 'number') {
    clearInterval(intervalId);
    intervalId = undefined;
  }
}

onBeforeUnmount(() => slideshowStop());

onMounted(() => {
  showAppBar.value = false;
  slideshowStart();
});
</script>

<template>
  <div class="home-view">
    <v-card class="crd-title" variant="flat">
      <img src="/title.svg" width="100%" />
    </v-card>

    <DivHomeMessages class="div-home-messages" v-if="inMobileMode" height="320" />

    <template v-else>
      <v-card class="crd-desktop">
        <Transition>
          <img :src="imgSlideSrc[imgSlideSrcIndex]" :key="imgSlideSrcIndex" class="img-banner" />
        </Transition>
        <DivHomeMessages class="div-home-messages" />
      </v-card>
      <b class="text-disabled">
        The images displayed on the card were AI generated and are for illustrative purposes only.
      </b>
    </template>
  </div>
</template>

<style scoped>
.crd-desktop {
  justify-content: space-evenly;
  max-width: 120dvh;
  margin: 64px 0;
  display: flex;
  height: 60dvh;
  width: 100%;
}

.crd-title {
  align-items: center;
  min-width: 320px;
  display: flex;
  padding: 8px;
  width: 50%;
}

.div-home-messages {
  position: absolute;
  height: 100%;
  width: 50%;
  right: 0;
}

.home-view {
  justify-content: space-evenly;
  flex-direction: column;
  align-items: center;
  text-align: center;
  display: flex;
  height: 100%;
}

.img-banner {
  position: absolute;
  height: 60dvh;
  width: 60dvh;
  left: 0;
}

.v-enter-active,
.v-leave-active {
  transition: opacity 2.5s ease-in-out;
}

.v-enter-from,
.v-leave-to {
  opacity: 0;
}

@media (max-width: 1024px) {
  .crd-title {
    width: calc(100% - 64px);
    margin: 32px 0 0;
  }

  .div-home-messages {
    position: relative;
    width: 100%;
  }
}
</style>
