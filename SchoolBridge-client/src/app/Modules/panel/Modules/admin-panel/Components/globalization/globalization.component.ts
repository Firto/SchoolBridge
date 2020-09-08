import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { GlobalizationService } from 'src/app/Modules/globalization/Services/globalization.service';
import { GlobalizationEditService } from 'src/app/Modules/globalization/Services/globalization-edit.service';
import { GlobalizationInfoService } from 'src/app/Modules/globalization/Services/globalization-info.service';

@Component({
    selector: "adm-globalization",
    styleUrls: ['./globalization.component.css'],
    templateUrl: './globalization.component.html'
})

export class GlobalizationComponent {
    addForm: FormGroup;

    constructor(public gbeService: GlobalizationEditService,
                public gbiService: GlobalizationInfoService,
                private fb: FormBuilder){
        this.addForm = this.fb.group({
            abbName: [''],
            fullName: ['']
        });
    }

    public addLanguage(){
        if (this.addForm.valid) 
            this.gbeService.addLanguage(this.addForm.controls.abbName.value, this.addForm.controls.fullName.value).subscribe(
                result => {
                if (result.ok == true){
                    this.addForm.reset();
                    document.getElementById("add-lang-btn").click();
                }
                else if (result.result.id == "v-dto-invalid"){
                    for (const [key, value] of Object.entries(result.result.additionalInfo)) 
                    this.addForm.controls[key].setErrors({"err":value});
                }
            });
    }

    public removeLanguage(e: MouseEvent){
        this.gbeService.removeLanguage((<any>e.target).title).subscribe();
    }
    
    public toggleEditingMode(){
        this.gbeService.state = !this.gbeService.state;
    }
}