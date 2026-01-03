import { Component, Input, input, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl, ReactiveFormsModule } from '@angular/forms';
import { MatError, MatInput, MatLabel, MatFormField } from '@angular/material/input';

@Component({
  selector: 'app-text-input',
  imports: [
    ReactiveFormsModule,
    MatInput,
    MatError,
    MatLabel,
    MatFormField
  ],
  templateUrl: './text-input.component.html',
  styleUrl: './text-input.component.scss',
})
export class TextInputComponent implements ControlValueAccessor {
  @Input() label = '';
  @Input() type = 'text';

  // Unique instance of this component
  // “Hãy inject NgControl gắn với chính component này vào biến controlDir”.
  // @Self() ép Angular chỉ tìm NgControl ngay trên instance này, không đi tìm ở cha, tránh lẫn với control khác
  constructor(@Self() public controlDir: NgControl) {
    // every get/set action from FormControl will have to go through this component to handle
    this.controlDir.valueAccessor = this;
  }

  writeValue(obj: any): void {

  }
  registerOnChange(fn: any): void {

  }
  registerOnTouched(fn: any): void {

  }
  setDisabledState?(isDisabled: boolean): void {

  }

  get control() {
    return this.controlDir.control as FormControl;
  }
}
