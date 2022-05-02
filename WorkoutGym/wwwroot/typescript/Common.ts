$.extend(jQuery, {
    dataJs: (selector: string) => {
       return $("[data-js='" + selector + "']");
    }
});

$.extend(jQuery, {
    currentDate: () => {
        
        let today = new Date();
        
        return new Date(today.getFullYear(), today.getMonth(), today.getDate()) ;
    }
});

$.extend(jQuery, {
    
    startCurrentMonthDate: () => {
        
        let today = new Date();
        
        return new Date(today.getFullYear(), today.getMonth(), 1);
    }
});

$.extend(jQuery, {
    
    endCurrentMonthDate: () => {
        
        let today = new Date();
        
        return new Date(today.getFullYear(), today.getMonth() + 1, 0);
    }
})