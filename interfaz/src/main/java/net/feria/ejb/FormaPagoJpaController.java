/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package net.feria.ejb;

import java.io.Serializable;
import javax.persistence.Query;
import javax.persistence.EntityNotFoundException;
import javax.persistence.criteria.CriteriaQuery;
import javax.persistence.criteria.Root;
import net.feria.entity.FacturasCabecera;
import java.util.ArrayList;
import java.util.Collection;
import java.util.List;
import javax.persistence.EntityManager;
import javax.persistence.EntityManagerFactory;
import net.feria.ejb.exceptions.IllegalOrphanException;
import net.feria.ejb.exceptions.NonexistentEntityException;
import net.feria.entity.FormaPago;

/**
 *
 * @author Deth1
 */
public class FormaPagoJpaController implements Serializable {

    public FormaPagoJpaController(EntityManagerFactory emf) {
        this.emf = emf;
    }
    private EntityManagerFactory emf = null;

    public EntityManager getEntityManager() {
        return emf.createEntityManager();
    }

    public void create(FormaPago formaPago) {
        if (formaPago.getFacturasCabeceraCollection() == null) {
            formaPago.setFacturasCabeceraCollection(new ArrayList<FacturasCabecera>());
        }
        EntityManager em = null;
        try {
            em = getEntityManager();
            em.getTransaction().begin();
            Collection<FacturasCabecera> attachedFacturasCabeceraCollection = new ArrayList<FacturasCabecera>();
            for (FacturasCabecera facturasCabeceraCollectionFacturasCabeceraToAttach : formaPago.getFacturasCabeceraCollection()) {
                facturasCabeceraCollectionFacturasCabeceraToAttach = em.getReference(facturasCabeceraCollectionFacturasCabeceraToAttach.getClass(), facturasCabeceraCollectionFacturasCabeceraToAttach.getCodigoFactura());
                attachedFacturasCabeceraCollection.add(facturasCabeceraCollectionFacturasCabeceraToAttach);
            }
            formaPago.setFacturasCabeceraCollection(attachedFacturasCabeceraCollection);
            em.persist(formaPago);
            for (FacturasCabecera facturasCabeceraCollectionFacturasCabecera : formaPago.getFacturasCabeceraCollection()) {
                FormaPago oldFormaPagoOfFacturasCabeceraCollectionFacturasCabecera = facturasCabeceraCollectionFacturasCabecera.getFormaPago();
                facturasCabeceraCollectionFacturasCabecera.setFormaPago(formaPago);
                facturasCabeceraCollectionFacturasCabecera = em.merge(facturasCabeceraCollectionFacturasCabecera);
                if (oldFormaPagoOfFacturasCabeceraCollectionFacturasCabecera != null) {
                    oldFormaPagoOfFacturasCabeceraCollectionFacturasCabecera.getFacturasCabeceraCollection().remove(facturasCabeceraCollectionFacturasCabecera);
                    oldFormaPagoOfFacturasCabeceraCollectionFacturasCabecera = em.merge(oldFormaPagoOfFacturasCabeceraCollectionFacturasCabecera);
                }
            }
            em.getTransaction().commit();
        } finally {
            if (em != null) {
                em.close();
            }
        }
    }

    public void edit(FormaPago formaPago) throws IllegalOrphanException, NonexistentEntityException, Exception {
        EntityManager em = null;
        try {
            em = getEntityManager();
            em.getTransaction().begin();
            FormaPago persistentFormaPago = em.find(FormaPago.class, formaPago.getCodigoFormaPago());
            Collection<FacturasCabecera> facturasCabeceraCollectionOld = persistentFormaPago.getFacturasCabeceraCollection();
            Collection<FacturasCabecera> facturasCabeceraCollectionNew = formaPago.getFacturasCabeceraCollection();
            List<String> illegalOrphanMessages = null;
            for (FacturasCabecera facturasCabeceraCollectionOldFacturasCabecera : facturasCabeceraCollectionOld) {
                if (!facturasCabeceraCollectionNew.contains(facturasCabeceraCollectionOldFacturasCabecera)) {
                    if (illegalOrphanMessages == null) {
                        illegalOrphanMessages = new ArrayList<String>();
                    }
                    illegalOrphanMessages.add("You must retain FacturasCabecera " + facturasCabeceraCollectionOldFacturasCabecera + " since its formaPago field is not nullable.");
                }
            }
            if (illegalOrphanMessages != null) {
                throw new IllegalOrphanException(illegalOrphanMessages);
            }
            Collection<FacturasCabecera> attachedFacturasCabeceraCollectionNew = new ArrayList<FacturasCabecera>();
            for (FacturasCabecera facturasCabeceraCollectionNewFacturasCabeceraToAttach : facturasCabeceraCollectionNew) {
                facturasCabeceraCollectionNewFacturasCabeceraToAttach = em.getReference(facturasCabeceraCollectionNewFacturasCabeceraToAttach.getClass(), facturasCabeceraCollectionNewFacturasCabeceraToAttach.getCodigoFactura());
                attachedFacturasCabeceraCollectionNew.add(facturasCabeceraCollectionNewFacturasCabeceraToAttach);
            }
            facturasCabeceraCollectionNew = attachedFacturasCabeceraCollectionNew;
            formaPago.setFacturasCabeceraCollection(facturasCabeceraCollectionNew);
            formaPago = em.merge(formaPago);
            for (FacturasCabecera facturasCabeceraCollectionNewFacturasCabecera : facturasCabeceraCollectionNew) {
                if (!facturasCabeceraCollectionOld.contains(facturasCabeceraCollectionNewFacturasCabecera)) {
                    FormaPago oldFormaPagoOfFacturasCabeceraCollectionNewFacturasCabecera = facturasCabeceraCollectionNewFacturasCabecera.getFormaPago();
                    facturasCabeceraCollectionNewFacturasCabecera.setFormaPago(formaPago);
                    facturasCabeceraCollectionNewFacturasCabecera = em.merge(facturasCabeceraCollectionNewFacturasCabecera);
                    if (oldFormaPagoOfFacturasCabeceraCollectionNewFacturasCabecera != null && !oldFormaPagoOfFacturasCabeceraCollectionNewFacturasCabecera.equals(formaPago)) {
                        oldFormaPagoOfFacturasCabeceraCollectionNewFacturasCabecera.getFacturasCabeceraCollection().remove(facturasCabeceraCollectionNewFacturasCabecera);
                        oldFormaPagoOfFacturasCabeceraCollectionNewFacturasCabecera = em.merge(oldFormaPagoOfFacturasCabeceraCollectionNewFacturasCabecera);
                    }
                }
            }
            em.getTransaction().commit();
        } catch (Exception ex) {
            String msg = ex.getLocalizedMessage();
            if (msg == null || msg.length() == 0) {
                Integer id = formaPago.getCodigoFormaPago();
                if (findFormaPago(id) == null) {
                    throw new NonexistentEntityException("The formaPago with id " + id + " no longer exists.");
                }
            }
            throw ex;
        } finally {
            if (em != null) {
                em.close();
            }
        }
    }

    public void destroy(Integer id) throws IllegalOrphanException, NonexistentEntityException {
        EntityManager em = null;
        try {
            em = getEntityManager();
            em.getTransaction().begin();
            FormaPago formaPago;
            try {
                formaPago = em.getReference(FormaPago.class, id);
                formaPago.getCodigoFormaPago();
            } catch (EntityNotFoundException enfe) {
                throw new NonexistentEntityException("The formaPago with id " + id + " no longer exists.", enfe);
            }
            List<String> illegalOrphanMessages = null;
            Collection<FacturasCabecera> facturasCabeceraCollectionOrphanCheck = formaPago.getFacturasCabeceraCollection();
            for (FacturasCabecera facturasCabeceraCollectionOrphanCheckFacturasCabecera : facturasCabeceraCollectionOrphanCheck) {
                if (illegalOrphanMessages == null) {
                    illegalOrphanMessages = new ArrayList<String>();
                }
                illegalOrphanMessages.add("This FormaPago (" + formaPago + ") cannot be destroyed since the FacturasCabecera " + facturasCabeceraCollectionOrphanCheckFacturasCabecera + " in its facturasCabeceraCollection field has a non-nullable formaPago field.");
            }
            if (illegalOrphanMessages != null) {
                throw new IllegalOrphanException(illegalOrphanMessages);
            }
            em.remove(formaPago);
            em.getTransaction().commit();
        } finally {
            if (em != null) {
                em.close();
            }
        }
    }

    public List<FormaPago> findFormaPagoEntities() {
        return findFormaPagoEntities(true, -1, -1);
    }

    public List<FormaPago> findFormaPagoEntities(int maxResults, int firstResult) {
        return findFormaPagoEntities(false, maxResults, firstResult);
    }

    private List<FormaPago> findFormaPagoEntities(boolean all, int maxResults, int firstResult) {
        EntityManager em = getEntityManager();
        try {
            CriteriaQuery cq = em.getCriteriaBuilder().createQuery();
            cq.select(cq.from(FormaPago.class));
            Query q = em.createQuery(cq);
            if (!all) {
                q.setMaxResults(maxResults);
                q.setFirstResult(firstResult);
            }
            return q.getResultList();
        } finally {
            em.close();
        }
    }

    public FormaPago findFormaPago(Integer id) {
        EntityManager em = getEntityManager();
        try {
            return em.find(FormaPago.class, id);
        } finally {
            em.close();
        }
    }

    public int getFormaPagoCount() {
        EntityManager em = getEntityManager();
        try {
            CriteriaQuery cq = em.getCriteriaBuilder().createQuery();
            Root<FormaPago> rt = cq.from(FormaPago.class);
            cq.select(em.getCriteriaBuilder().count(rt));
            Query q = em.createQuery(cq);
            return ((Long) q.getSingleResult()).intValue();
        } finally {
            em.close();
        }
    }
    
}
