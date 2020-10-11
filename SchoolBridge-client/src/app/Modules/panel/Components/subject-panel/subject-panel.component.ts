import { DOCUMENT } from "@angular/common";
import {
  Component,
  OnInit,
  ChangeDetectionStrategy,
  Inject,
  Renderer2,
} from "@angular/core";
import { MdGlobalization } from "src/app/Modules/globalization/Services/md-globalization.service";
import { SubjectService } from "../../Services/subject.service";

@Component({
  selector: "app-subject-panel",
  templateUrl: "./subject-panel.component.html",
  styleUrls: ["./subject-panel.component.css"],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: MdGlobalization("sb-panel"),
})
export class SubjectPanelComponent implements OnInit {
  constructor(
    private _subService: SubjectService,
    @Inject(DOCUMENT) private _document: Document,
    private _render: Renderer2
  ) {}

  ngOnInit(): void {
    this._subService.getAll().subscribe((x) => {
      x.forEach((x) => {
        (<any>this._document.getElementById(`sub-${x.day}-${x.lesson}`)).value = x.lessonName;
        (<any>this._document.getElementById(`desc-${x.day}-${x.lesson}`)).value = x.description;
      });
    });
  }

  change(day: number, lesson: number) {
    console.log(day, lesson)
    console.dir(this._document.getElementById(`sub-${day}-${lesson}`), this._document.getElementById(`desc-${day}-${lesson}`));
    this._subService.addOrUpdate(
      day,
      lesson,
      (<any>this._document.getElementById(`sub-${day}-${lesson}`)).value,
      (<any>this._document.getElementById(`desc-${day}-${lesson}`)).value
    ).subscribe();
  }
}
