using System.Collections.Generic;
using System.Windows;

namespace CustomWpfControls.LayoutStrategies
{
    /// <summary>
    /// Стратегия отображения дочерних элементов на панели.
    /// </summary>
    public interface ILayoutStrategy
    {
        /// <summary>
        /// Размер панели. 
        /// </summary>
        Size ResultSize { get; }

        /// <summary>
        /// Вычислить размеры панели и расположение дочерних элементов.
        /// </summary>
        /// <param name="availablePanelSize">Доступный размер панели.</param>
        /// <param name="sizes">Размеры дочерних элементов.</param>
        /// <param name="isDragging">Признак, что выполняется перенос элементов.</param>
        void MeasureLayout(Size availablePanelSize, List<Size> sizes, bool isDragging);

        /// <summary>
        /// Возвращает индекс дочернего элемента по указанным координатам.
        /// </summary>
        /// <param name="position">Координаты, по которым расположен дочерний элемент.</param>
        int GetIndex(Point position);

        /// <summary>
        /// Возвращает информацию о расположении элемента по его индексу.
        /// </summary>
        /// <param name="index">Индекс элемента.</param>
        ItemLayoutInfo GetLayoutInfo(int index);
    }
}