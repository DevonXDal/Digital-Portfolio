import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ContactMessage } from './data/contact-message';
import { MessageService } from 'primeng/api';
import { ContactMeApiServiceService } from './contact-me-api-service.service';
import { AuthService } from '@auth0/auth0-angular';
import { Observable, map } from 'rxjs';
import { InsertedUserInfo } from './data/inserted-user-info';

@Component({
  selector: 'app-contact-me',
  templateUrl: './contact-me.component.html',
  styleUrls: ['./contact-me.component.scss']
})
export class ContactMeComponent implements OnInit {
  model = new ContactMessage("", "", "", undefined);
  isLoading = false;
  userInfo: InsertedUserInfo | null = null;
  
  timer = setTimeout(() => { // Load user data for the form after one second.
    this.resetModel();
  }, 1000)

  constructor(
    private toastService: MessageService, 
    private apiService: ContactMeApiServiceService,
    private authService: AuthService) { }

  ngOnInit(): void {
    // Try to grab the user's name, email, and phone number (if available), if they are signed in, or return null if they are not.
    this.authService.user$.pipe
      (
        map(u => (u) 
          ? this.userInfo = {
            name: u.name!,
            email: u.email!,
            phone: u.phone_number
          } 
          : this.userInfo = null
        )
      );
    
    
  }

  async sendEmail(contactForm: NgForm): Promise<void> {
    if (contactForm.invalid) {
      this.toastService.add({key: 'tr', severity: 'error', summary: 'Form is Invalid', detail: "The name, email, and message fields are required."});
      return;
    }

    this.isLoading = true;
    let serviceResponse: string = await this.apiService.sendContactForm(this.model);

    if (serviceResponse) {
      this.toastService.add({key: 'tr', severity: 'error', summary: 'Attempt Failed', detail: serviceResponse});
    } else {
      this.toastService.add({key: 'tr', severity: 'success', summary: 'Sent Form', detail: "The form has been sent."});
      this.resetModel(); 
    }

    this.isLoading = false;
  }

  // Reset the data in the contact form to either be blank or include the user's account information.
  private resetModel(): ContactMessage {
    if (this.userInfo) { // Is there an authenticated user?
      return new ContactMessage(this.userInfo.email, this.userInfo.name, "", this.userInfo.phone);
    }

    return new  ContactMessage("", "", "", undefined);
  }

}
