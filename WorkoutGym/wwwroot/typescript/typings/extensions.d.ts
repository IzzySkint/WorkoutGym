interface JQuery{
    dataJs(selector: string): JQuery;
    selectize(options?:any): JQuery;
    currentDate(): Date;
    startCurrentMonthDate(): Date;
    endCurrentMonthDate(): Date;
}

interface HTMLElement{
    selectize: any;
    addOption(option: any): void;
    clearOptions(silent: boolean): void;
    refreshOptions(triggerDropdown: boolean): void;
    clear(silent: boolean);
    getValue(): string;
    setValue(value: any, silent: boolean);
}

interface JQueryStatic{
    dataJs(selector: string): JQuery;
    currentDate(): Date;
    startCurrentMonthDate(): Date;
    endCurrentMonthDate(): Date;
}
