import { useLocalStorageRef } from '@/utils/refUtil';
import { defineStore } from 'pinia';
import { ref } from 'vue';

export const useConfigStore = defineStore('config', () => {
  const inDarkMode = useLocalStorageRef<boolean | undefined>('in-dark-mode');
  const inMobileMode = ref<boolean>(window.innerWidth <= 1024);
  const showAppBar = ref<boolean>(false);

  return {
    inDarkMode,
    inMobileMode,
    showAppBar
  };
});
