import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ProfileService } from '../../Services/profile.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})

export class SettingsComponent implements OnInit {
  settingsForm: FormGroup;
  ngOnInit(): void {
    this.settingsForm = this.fb.group({
      login: [''],
    });
  }
  public apiUrl: string = environment.apiUrl;
  constructor(private fb: FormBuilder,
    public profileService: ProfileService) { }

  changeLogin() {
    if (this.settingsForm.valid) {
      this.profileService.changeLogin(this.settingsForm.controls.login.value).subscribe(res => {}, err => {
        for (const [key, value] of Object.entries(err.additionalInfo))
          this.settingsForm.controls[key].setErrors({ "err": value });
      });
    }}
}