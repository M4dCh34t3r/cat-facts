<script setup lang="ts">
import { ref } from 'vue';
import { VIcon } from 'vuetify/components';

const cpnIcon = ref<typeof VIcon>();

const emit = defineEmits<{
  (e: 'click'): void;
}>();

function click() {
  const el = cpnIcon.value?.$el;
  el.classList.add('animate');
  el.addEventListener('animationend', () => el.classList.remove('animate'));
  emit('click');
}

defineProps<{
  amount: number;
  color?: string;
  disabled?: boolean;
  location?: 'top' | 'bottom' | 'left' | 'right';
  text: string;
}>();
</script>

<template>
  <v-tooltip :location="location || 'left'" :text="text">
    <template #activator="{ props }">
      <v-badge :content="amount" color="success" offset-x="2" offset-y="2" max="99">
        <v-btn
          :color="color || 'grey'"
          :disabled="disabled"
          @click="click"
          size="x-small"
          v-bind="props"
          class="ma-1"
          icon
        >
          <v-icon ref="cpnIcon" size="24">{{ 'mdi-thumb-up' }}</v-icon>
        </v-btn>
      </v-badge>
    </template>
  </v-tooltip>
</template>

<style scoped>
.animate {
  animation: animation 0.5s ease-in-out;
}

@keyframes animation {
  0%,
  100% {
    transform: rotate(0) scale(1);
  }
  33.33% {
    transform: rotateX(45deg) rotateZ(-45deg) scale(1.5);
  }
}
</style>
