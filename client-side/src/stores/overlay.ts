import { ServiceExceptionCategory } from '@/enums';
import { defineStore } from 'pinia';

export const useOverlayStore = defineStore('orverlay', {
  state: () => ({
    dlgLoading: false,
    dlgMessage: false,
    dlgMessageColor: '',
    dlgMessageIcon: '',
    dlgMessageTitle: '',
    dlgMessageText: '',
    dlgServerError: false
  }),
  actions: {
    hideLoadingDialog() {
      this.dlgLoading = false;
    },
    hideMessageDialog() {
      this.dlgMessage = false;
    },
    hideServerErrorDialog() {
      this.dlgServerError = false;
    },
    showMessageDialog(
      category: ServiceExceptionCategory,
      title: string,
      text: string,
      delay: number = 0
    ) {
      if (category === ServiceExceptionCategory.Ignore) return;

      const categoryConfig = {
        [ServiceExceptionCategory.Ignore]: { icon: 'mdi-dots-horizontal-circle', color: 'grey' },
        [ServiceExceptionCategory.Warning]: { icon: 'mdi-alert', color: 'warning' },
        [ServiceExceptionCategory.Error]: { icon: 'mdi-close-octagon', color: 'error' },
        [ServiceExceptionCategory.Information]: { icon: 'mdi-information-box', color: 'info' },
        [ServiceExceptionCategory.Success]: { icon: 'mdi-check-decagram', color: 'success' }
      };

      const { icon, color } = categoryConfig[category];

      setTimeout(() => (this.dlgMessage = true), delay);

      this.dlgMessageColor = color;
      this.dlgMessageIcon = icon;
      this.dlgMessageText = text;
      this.dlgMessageTitle = title;
    },
    showErrorMessageDialog(titulo: string, texto: string, delay: number = 0) {
      this.showMessageDialog(ServiceExceptionCategory.Error, titulo, texto, delay);
    },
    showInfoMessageDialog(titulo: string, texto: string, delay: number = 0) {
      this.showMessageDialog(ServiceExceptionCategory.Information, titulo, texto, delay);
    },
    showLoadingDialog() {
      this.dlgLoading = true;
    },
    showServerErrorDialog(delay: number = 0) {
      setTimeout(() => (this.dlgServerError = true), delay);
    },
    showSuccessMessageDialog(titulo: string, texto: string, delay: number = 0) {
      this.showMessageDialog(ServiceExceptionCategory.Success, titulo, texto, delay);
    },
    showWarningMessageDialog(titulo: string, texto: string, delay: number = 0) {
      this.showMessageDialog(ServiceExceptionCategory.Warning, titulo, texto, delay);
    }
  }
});
