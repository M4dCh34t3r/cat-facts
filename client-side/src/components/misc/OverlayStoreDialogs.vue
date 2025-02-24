<script setup lang="ts">
import { useOverlayStore } from '@/stores';
import { storeToRefs } from 'pinia';
import TbrWindow from '../toolbars/TbrWindow.vue';

const overlayStore = useOverlayStore();
const { dlgLoading, dlgMessage, dlgServerError } = storeToRefs(useOverlayStore());

function btnReloadClick() {
  setTimeout(() => location.reload(), 500);
  overlayStore.hideServerErrorDialog();
}
</script>

<template>
  <v-dialog v-model="dlgLoading" class="text-center" persistent>
    <h2>{{ 'Loading...' }}</h2>
    <v-progress-circular class="mx-auto" indeterminate size="96" width="6">
      <template #default>
        <v-icon class="icn-timer-sand-complete" icon="mdi-timer-sand-complete" size="64" />
      </template>
    </v-progress-circular>
  </v-dialog>

  <v-dialog v-model="dlgMessage" width="512">
    <v-card :color="overlayStore.dlgMessageColor" class="bg-surface" variant="outlined">
      <TbrWindow
        @btn-close-click="() => (dlgMessage = false)"
        :color="overlayStore.dlgMessageColor"
        :title="overlayStore.dlgMessageTitle"
      />
      <v-card-text class="overflow-y-auto font-weight-bold text-center pa-2">
        {{ overlayStore.dlgMessageText }}
      </v-card-text>
      <v-icon :icon="overlayStore.dlgMessageIcon" class="mx-auto mb-2" size="48" />
      <v-btn
        @click="overlayStore.hideMessageDialog()"
        :color="overlayStore.dlgMessageColor"
        class="mx-auto mb-4"
        density="compact"
        variant="flat"
        width="5rem"
        text="ok"
      />
    </v-card>
  </v-dialog>

  <v-dialog v-model="dlgServerError" max-width="400">
    <v-card color="background" variant="flat">
      <TbrWindow
        @btn-close-click="overlayStore.hideServerErrorDialog()"
        title="WARNING!"
        color="surface"
      />
      <h4 class="text-center mx-8 my-2">AN INTERNAL SERVER ERROR HAS HAPPENED</h4>
      <v-progress-circular class="mx-auto" indeterminate size="80" width="6">
        <template #default>
          <v-icon class="icn-web" icon="mdi-web" size="64" />
        </template>
      </v-progress-circular>
      <h4 class="text-center mt-2">PLEASE...<br />CONSIDER CHECKING THE LOGS</h4>
      <v-row no-gutters>
        <v-btn
          @click="btnReloadClick"
          class="mx-auto my-2"
          density="compact"
          width="7.5rem"
          text="reload"
        />
      </v-row>
    </v-card>
  </v-dialog>
</template>

<style scoped>
.icn-timer-sand-complete {
  animation: icn-timer-sand-complete-rotate 2.5s infinite ease-in-out;
}

.icn-web {
  animation: icn-web-spin 10s infinite linear;
  transform-origin: bottom center;
}

@keyframes icn-web-spin {
  0% {
    transform: rotateY(0);
  }
  50% {
    transform: rotateY(360deg);
  }
  100% {
    transform: rotateY(0);
  }
}

@keyframes icn-timer-sand-complete-rotate {
  0% {
    transform: scale(1.25) rotateZ(0);
  }
  2.5% {
    transform: scale(1) rotateZ(0);
  }
  5% {
    transform: scale(0.75) rotateZ(0);
  }
  7.5% {
    transform: scale(1) rotateZ(0);
  }
  10% {
    transform: scale(1) rotateZ(0);
  }
  25% {
    transform: rotateZ(180deg);
  }
  100% {
    transform: rotateZ(180deg);
  }
}
</style>
