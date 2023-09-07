import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NgForm } from '@angular/forms';

import { environment as env } from '../../environments/environment';
import { ContactMessage } from './data/contact-message';
import { lastValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ContactMeApiServiceService {
  private httpFormOptions: Object = { // Must be typed to Object
    observe: 'response',
    responseType: 'text'
  };

  constructor(private http: HttpClient) { }

  async sendContactForm(data: ContactMessage): Promise<string> {
    const url = `${env.api.serverUrl}/api/v${env.api.versionUsed}/ContactForm`;

    let formData = new FormData();
    let response: HttpErrorResponse | HttpResponse<any> | null = null;

    formData.append('Name', data.name);
    formData.append('Email', data.email);
    if (data.phone) formData.append('Phone', data.phone);
    formData.append('Message', data.message);

    try {
      response = await lastValueFrom(
        this.http.put<HttpResponse<any> | HttpErrorResponse>(url, formData, this.httpFormOptions), {defaultValue: null}
      );
    } catch (err) { // Likely a Bad Request 400
      response = err as HttpErrorResponse;
    }

    if (!response) return Promise.resolve("No response from the API"); // No response
    if (response.status == 200) return Promise.resolve(""); // Ok 200
    
    return Promise.resolve("Failed to call the API to send the information."); // Bad Request
  }
}
