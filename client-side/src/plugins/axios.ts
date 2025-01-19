import { assertType } from '@/helpers/typeAssertHelper';
import { useOverlayStore } from '@/stores';
import type { ServiceExceptionDTO } from '@/typings';
import axios, { AxiosError, HttpStatusCode, type AxiosInstance, type AxiosResponse } from 'axios';
import type { Plugin } from 'vue';
import router from './router';

const vueAxios: Plugin = {
  install(app) {
    const instance: AxiosInstance = axios.create({
      timeout: import.meta.env.DEV ? 60000 : 30000,
      paramsSerializer: { indexes: null },
      baseURL: '/Api'
    });

    instance.interceptors.response.use(
      async (response) => {
        handleSuccessResponse(response);
        return response;
      },
      async (error) => handleErrorResponse(error)
    );

    app.provide('axios', instance);
  }
};

function handleErrorResponse(error: AxiosError) {
  if (error.response) {
    if (!handleServiceException(error.response)) handleResponseStatus(error.response.status);
  } else useOverlayStore().showServerErrorDialog();
}

function handleResponseStatus(status: HttpStatusCode) {
  if (status === HttpStatusCode.Forbidden) {
    router.push('/');
    useOverlayStore().showInfoMessageDialog(
      'You cannot view this content',
      'Due to security reasons you have been redirected'
    );
  } else if (status === HttpStatusCode.InternalServerError)
    useOverlayStore().showServerErrorDialog();
}

function handleServiceException(res: AxiosResponse) {
  if (
    res.data instanceof Object &&
    'category' in res.data &&
    'title' in res.data &&
    'text' in res.data
  ) {
    const serviceException = assertType<ServiceExceptionDTO>({ serviceExceptionDTO: res.data });
    if (serviceException.category && serviceException.text && serviceException.title) {
      useOverlayStore().showMessageDialog(
        serviceException.category,
        serviceException.title,
        serviceException.text
      );
      return true;
    }
  }
  return false;
}

function handleSuccessResponse(response: AxiosResponse) {
  handleServiceException(response.data);
  return response;
}

export default vueAxios;
