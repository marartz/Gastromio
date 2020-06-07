export class PagingModel<TItem> {
  constructor() {
  }

  public total: number;

  public skipped: number;

  public taken: number;

  public items: TItem[];
}
