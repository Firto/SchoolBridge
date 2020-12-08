import { ChangeDetectorRef } from "@angular/core";
import { Input } from "@angular/core";
import { BehaviorSubject, Observable, Subject, Subscription } from "rxjs";
import { debounceTime, finalize, tap } from "rxjs/operators";

export class NgxFormArgModel {
  private _errors: Observable<string>[] = [];

  public get name(): string{
    return this.info.name;
  }

  public get type(): string{
    return this.info.type;
  }

  constructor(
    public readonly info: {name: string, type: string},
    private readonly _sb: Subject<unknown>
  ) {}

  public setErrors(errors: Observable<string>[]): void {
    this._errors = errors;
  }a

  public clearErrors(): void {
    this._errors = [];
  }

  public clearErrorsD(): void {
    this._errors = [];
    this._sb.next();
  }

  public hasErrors(): boolean {
    return this._errors.length > 0;
  }

  public get errors(): Observable<string>[] {
    return this._errors;
  }

  public get value(): string {
    return (<any>document.getElementById(this.name)).value;
  }
}

export class NgxFormModel {
  public readonly args: Record<string, NgxFormArgModel> = {};
  public get argsV(): NgxFormArgModel[] {
    return Object.values(this.args);
  }
  private _sb: Subject<unknown> = new Subject();
  public readonly onChanged: Observable<unknown> = this._sb.pipe(
    debounceTime(100)
  );

  constructor(public readonly name: string, public readonly args_: (string | {name: string, type: string})[]) {
    args_.forEach((x) => {
      if (typeof x === 'string')
        this.args[x] = new NgxFormArgModel({name: x, type: 'text'}, this._sb);
      else this.args[x.name] = new NgxFormArgModel(x, this._sb);
    });
  }

  public setErrors(errors: Record<string, Observable<string>[]>): void {
    Object.keys(errors).forEach((x) => {
      this.args[x].setErrors(
        errors[x].map((r) => r.pipe(tap(() => this._sb.next())))
      );
    });
    this._sb.next();
  }

  public clearErrors(): void {
    Object.keys(this.args).forEach((x) => {
      this.args[x].clearErrors();
    });
    this._sb.next();
  }

  public get valid(): boolean {
    return Object.keys(this.args).filter((x) => this.args[x].hasErrors()).length === 0;
  }

  public createObj(): Record<string, any> {
    const ss: Record<string, any> = {};
    Object.keys(this.args).forEach(x => ss[x] = this.args[x].value);
    return ss;
  }
}