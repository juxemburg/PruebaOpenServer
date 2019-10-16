import { animation, style, animate } from '@angular/animations';

export const fadeInAnimation = animation(
  [
    style({ opacity: '0', transform: 'translateY({{ initialYPos }})' }),
    animate(
      '{{ duration }} {{ easeFn }}',
      style({ opacity: '1', transform: 'translateY(0)' })
    ),
  ],
  { params: { initialYPos: '-20px', duration: '200ms', easeFn: 'ease-out' } }
);
