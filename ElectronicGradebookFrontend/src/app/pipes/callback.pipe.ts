import { Pipe, PipeTransform } from '@angular/core'

@Pipe({
  name: 'callback'
})
export class CallbackPipe implements PipeTransform {
  transform(items: any[], callback: (item: any, ...args: any[]) => boolean): any {
    if (!items || !callback) return items
    return items.filter(item => callback(item))
  }
}
