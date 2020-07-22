import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ProfileService } from '../../Services/profile.service';

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

  constructor(private fb: FormBuilder,
    private _profileService: ProfileService) { }

  changeLogin() {
    if (this.settingsForm.valid) {
      this._profileService.changeLogin(this.settingsForm.controls.login.value).subscribe(res => {}, err => {
        for (const [key, value] of Object.entries(err.additionalInfo))
          this.settingsForm.controls[key].setErrors({ "err": value });
      });
    }}
}