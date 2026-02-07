import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const accessToken = localStorage.getItem('AccessToken');
  if (accessToken) {
    const cloneReq = req.clone({
      setHeaders: { Authorization: 'Bearer ' + accessToken },
    });
    req = cloneReq;
  }
  return next(req);
};
