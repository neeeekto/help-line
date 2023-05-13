# tickets-query-language 
Языковая модель для поискового запроса тикетов (парсер, лексер, suggest)

## Примеры использования
```
id=0-123345 & assigment=none & tags=[tag1]
id=0-123345 and assigment=none and tags=[tag1]
```

## Синтаксис группировки
- `filter1 and filter2 and filter3` = пересечение фильтров
- `(filter1 or filter2)` = объеденение фильтров

Также для операторов присутствуют аналоги
- `or` = `|` 
- `and` = `&` 

## Синтаксис значений

### Date Value

Выражение `[Key][Operator][Value]{Action}`

Где:
- `Key` - ключ поля (см конкретный фильтр)
- `Operator` - условие одно из `<` `<=` `>` `>=` `=`
- `Value` - значение времени, возможные 
  - `now` - текущее время, т.е время на момент запуска фильтра
  - `DD.MM.YYYY hh:mm:ss` - точное время
- `Action` - опциональное поле действий с датой. Формат `[Operation][Amount]`, где
  - `Operation` - тип действия, `+` - прибавить, `-` - отнять
  - `Amount` - кол-во единиц времени, формата `[Count][Type]`, пример: `1h 23m 3s`
    - `Count` - кол-во
    - `Type` - тип еденицы: `h/m/s/M/Y`

Примеры
```
createDate=now + 1h
createDate=now - 12h

```

## Фильтры

### Фильтр по ID
`id=X-XXXXXX` 

### Фильтр по назначенному оператору
```
assigment=me
assigment=username
assigment=none
```

Значения
- `me` - текущий оператор (который выполняет запрос)
- `none` - не назначенный
- `@username` - конкретный оператор

### По аттачментам
`hasAttachment=true/false` 

### По дате создания
`createDate=[Date Value]`, см [Date Value](#date-value)

### Имеет hardAssigment
`hardAssigment=true/false`

### Кол-во итераций
`iterationCount > 1` 
`iterationCount < 1` 
`iterationCount >= 1` 
`iterationCount <= 1` 
`iterationCount = 1`

### По языку
`language=[lang1, lang2, lang3]`
`language=lang1`

### По типу последнего сообщению
`lastMessage=Incoming`
`lastMessage=Outgoin`

### По дате последнего ответа
`lastReply=[Date Value]`, см [Date Value](#date-value)

### По метаинфе
`meta=(fromTicket=[0-000000], sources=[t1, t2, t3], platforms=[android, ios])`

### По проекту
`project=XXX`

### По статусу
`status=Value{.Kind}`

- Где `value` может быть значением или массивом, варианты типов:
  - `New`
  - `Answered`
  - `AwaitingReply`
  - `Resolved`
  - `Rejected`
  - `ForReject`
- `Kind` - может быть значением или массивом, варианты типов
  - `Opened`
  - `Closed`
  - `Pending`
  
Примеры
```
status=New
status=[Answered, AwaitingReply]
status=New.Opened
status=[New, Answered].Opened
status=[Resolved, Rejected].[Closed]
```

### По тэгам
```
tags = [tag1, tag2] // include
tags != [tag1, tag2] // exlude
```
