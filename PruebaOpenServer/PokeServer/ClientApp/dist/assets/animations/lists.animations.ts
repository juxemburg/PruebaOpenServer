import {
  trigger,
  transition,
  query,
  stagger,
  animateChild,
  style,
  animate,
} from '@angular/animations';

export const listAnimations = [
  trigger('list', [
    transition(':enter', [
      query('@items', stagger(20, animateChild()), { optional: true }),
    ]),
  ]),
  trigger('items', [
    transition(':enter', [
      style({ transform: 'translateY(-10%)', opacity: 0 }),
      animate(
        '250ms cubic-bezier(.11,.03,.15,1.33)',
        style({ transform: 'translateY(0)', opacity: 1 })
      ),
    ]),
    transition(':leave', [
      style({ transform: 'translateY(0)', opacity: 1 }),
      animate(
        '200ms cubic-bezier(.53,-0.03,.35,.8)',
        style({
          transform: 'translateY(10%)',
          opacity: 0,
        })
      ),
    ]),
  ]),
];

export const listCascadeAnimations = [
  trigger('list', [
    transition(':enter', [
      query('@items', stagger(300, animateChild()), { optional: true }),
    ]),
  ]),
  trigger('items', [
    transition(':enter', [
      style({ transform: 'translateY(20%)', opacity: 0 }),
      animate(
        '300ms cubic-bezier(.11,.03,.15,1.33)',
        style({ transform: 'translateY(0)', opacity: 1 })
      ),
    ]),
    transition(':leave', [
      style({ transform: 'translateY(0)', opacity: 1 }),
      animate(
        '200ms cubic-bezier(.53,-0.03,.35,.8)',
        style({
          transform: 'translateY(-20%)',
          opacity: 0,
        })
      ),
    ]),
  ]),
];
export const listHorizontalCascadeAnimations = [
  trigger('list', [
    transition(':enter', [
      query('@items', stagger(300, animateChild()), { optional: true }),
    ]),
  ]),
  trigger('items', [
    transition(':enter', [
      style({ transform: 'translateX(-20%)', opacity: 0 }),
      animate(
        '300ms cubic-bezier(.11,.03,.15,1.33)',
        style({ transform: 'translateX(0)', opacity: 1 })
      ),
    ]),
    transition(':leave', [
      style({ transform: 'translateX(0)', opacity: 1 }),
      animate(
        '0ms cubic-bezier(.53,-0.03,.35,.8)',
        style({
          transform: 'translateX(20%)',
          opacity: 0,
        })
      ),
    ]),
  ]),
];
