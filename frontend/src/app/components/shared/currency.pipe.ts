import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'currencyPLN',
  standalone: true,
})
export class CurrencyPipe implements PipeTransform {
  transform(value: number | null, fractionDigits = 2): string {
    if (value === null || value === undefined) return '';
    const formatted = value
      .toFixed(fractionDigits)
      .replace('.', ',')
      .replace(/\B(?=(\d{3})+(?!\d))/g, ' ');
    return `${formatted} zł`;
  }
}
