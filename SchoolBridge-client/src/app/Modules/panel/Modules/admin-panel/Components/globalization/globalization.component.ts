import { Component } from '@angular/core';
import { GlobalizationService } from 'src/app/Modules/globalization/services/globalization.service';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
    selector: "adm-globalization",
    styleUrls: ['./globalization.component.css'],
    templateUrl: './globalization.component.html'
})

export class GlobalizationComponent {
    addForm: FormGroup;

    constructor(public globalizationService: GlobalizationService,
                private fb: FormBuilder){
        this.addForm = this.fb.group({
            abbName: [''],
            fullName: ['']
        });
    }

    public addLanguage(){
        if (this.addForm.valid) 
            this.globalizationService.addLanguage(this.addForm.controls.abbName.value, this.addForm.controls.fullName.value).subscribe(
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
        this.globalizationService.removeLanguage((<any>e.target).title).subscribe();
    }
}