import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const cloneReq = req.clone({
    withCredentials: true,
  });
  req = cloneReq;
  return next(cloneReq);
};
