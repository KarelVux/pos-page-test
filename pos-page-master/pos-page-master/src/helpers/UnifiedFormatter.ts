
export function getFormattedDate(date: Date,  formatRule = "et" ) {
        return (new Date(date).toLocaleString(formatRule)).toString()
}