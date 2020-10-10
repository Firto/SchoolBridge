import { ChangeDetectionStrategy, Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import { GlobalizationService } from "src/app/Modules/globalization/Services/globalization.service";
import { GlobalizationEditService } from "src/app/Modules/globalization/Services/globalization-edit.service";
import { GlobalizationInfoService } from "src/app/Modules/globalization/Services/globalization-info.service";
import { NgxFormModel } from "src/app/Modules/ngx-form/ngx-form.model";
import { observed } from "src/app/Decorators/observed.decorator";
import { combineLatest, merge, Observable } from "rxjs";
import { OnUnsubscribe } from "src/app/Services/super.controller";
import { map, takeUntil } from 'rxjs/operators';
import { MdGlobalization } from 'src/app/Modules/globalization/Services/md-globalization.service';
import { markDirty } from 'src/app/Helpers/mark-dirty.func';

@Component({
  selector: "adm-globalization",
  styleUrls: ["./globalization.component.css"],
  templateUrl: "./globalization.component.html",
  providers: MdGlobalization("gbs",
    [
      "pn-abbName",
      "pn-fullName",
      "cl-lss-lng-ald-reg",
      "lss-inc-lang-str"
    ]
  )
})
export class GlobalizationComponent extends OnUnsubscribe implements OnInit {
  public form: NgxFormModel = new NgxFormModel("form", ["abbName", "fullName"]);

  constructor(
    public gbeService: GlobalizationEditService,
    public gbiService: GlobalizationInfoService
  ) {
    super();
  }

  public ngOnInit(){
    combineLatest(this.form.onChanged, this.gbiService.infoObs).pipe(takeUntil(this._destroy)).subscribe(() => {
      markDirty(this);
      console.log("ROR");
    });
  }

  public addLanguage() {
    if (this.form.valid)
      this.gbeService
        .addLanguage(
          this.form.args.abbName.value,
          this.form.args.fullName.value
        )
        .subscribe(
          (val) => {},
          (err) => {
            if (err.id == "v-dto-invalid")
              this.form.setErrors(err.additionalInfo);
          }
        );
  }

  onChange(arg: string) {
    if (this.form.valid) return;
    this.form.args[arg].clearErrorsD();
  }

  public removeLanguage(e: MouseEvent) {
    this.gbeService.removeLanguage((<any>e.target).title).subscribe();
  }

  public toggleEditingMode() {
    this.gbeService.state = !this.gbeService.state;
  }
}
