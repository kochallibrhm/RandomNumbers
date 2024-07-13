import { Injectable } from '@angular/core';
import {
    HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {

    constructor(private toastr: ToastrService) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(req).pipe(
            catchError((error: HttpErrorResponse) => {
                debugger
                if (error.error instanceof ErrorEvent) {
                    // Client-side or network error
                    this.toastr.error(`Client error: ${error.error.message}`, 'Client Error');
                } else {
                    // Backend error
                    if (error.error?.errorMessage) {
                        this.toastr.error(error.error.errorMessage, 'Error');
                    } else {
                        // Other error
                        this.toastr.error(`Server error: ${error.status}`, 'Server Error');
                    }
                }
                return throwError(error);
            })
        );
    }
}
