import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import { ProfileService } from "../../Services/profile.service";
import { environment } from "src/environments/environment";
import { MdGlobalization } from "src/app/Modules/globalization/Services/md-globalization.service";
import { NgxFormModel } from "src/app/Modules/ngx-form/ngx-form.model";
import { ElementRef } from "@angular/core";
import { ViewChild } from "@angular/core";
import { takeUntil } from 'rxjs/operators';
import { OnUnsubscribe } from 'src/app/Services/super.controller';
import { markDirty } from 'src/app/Helpers/mark-dirty.func';

@Component({
  selector: "settings",
  templateUrl: "./settings.component.html",
  styleUrls: ["./settings.component.css"],
  providers: MdGlobalization("st"),
})
export class SettingsComponent extends OnUnsubscribe implements OnInit {
  public form: NgxFormModel = new NgxFormModel("form", ["login"]);
  private t_fileType: string;
  @ViewChild("login", { static: true }) public el_lg: ElementRef;

  public apiUrl: string = environment.apiUrl;
  constructor(public pService: ProfileService) {
    super();
  }

  ngOnInit(): void {
    this.form.onChanged.pipe(takeUntil(this._destroy)).subscribe(() => {
      markDirty(this);
    });
  }

  onChange(arg: string){
    if (this.form.valid) return;
    this.form.args[arg].clearErrorsD();
  }

  changed(evt: any) {
    const file: File = evt.target.files[0];

    if (file) {
      const reader = new FileReader();
      this.t_fileType = file.type;
      reader.onload = this.handleReaderLoaded.bind(this);
      reader.readAsBinaryString(file);
    }
  }

  handleReaderLoaded(e) {
      this.pService
        .changeImage('data:' + this.t_fileType + ';base64,' + btoa(e.target.result))
        .subscribe();
  }

  changeLogin() {
    if (this.form.valid) {
      this.pService
        .changeLogin(this.el_lg.nativeElement.value)
        .subscribe(
          (res) => {},
          err => {
            if (err.id == "v-dto-invalid")
              this.form.setErrors(err.additionalInfo);
          }
        );
    }
  }
}
